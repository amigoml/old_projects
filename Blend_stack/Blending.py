import numpy as np

from sklearn.cross_validation import cross_val_predict, StratifiedKFold
from sklearn.base import BaseEstimator

from cvxopt import solvers, matrix, spdiag, log, exp, div
from cvxopt.modeling import op
solvers.options['show_progress'] = False


def give_a(n):
    M = n.shape[0]
    N = n.shape[1]
    a = np.ones((1, N))
    return a


def give_g(n):
    M = n.shape[0]
    N = n.shape[1]
    a = np.zeros((N, N))
    for i in range(N):
        a[i,i] = -1
    return a


def give_b(n):
    b = matrix(1., (1,1))
    return b


def give_h(n):
    M = n.shape[0]
    N = n.shape[1]
    a = np.zeros((1, N))
    return a.T


def solver_cross_entr(n, y):
    # solver для минимизации logloss'a между выпуклой суммой столбцов (n) и ответами y. 
    Aa = matrix(give_a(n))
    bb = matrix(give_b(n))
    Gg = matrix(give_g(n))
    hh = matrix(give_h(n))
    O = matrix(n)
    m = O.size[0]
    X = matrix(1.0, (m,O.size[1]))
    X[:, 0:O.size[1]] = O
    
    def F(alph=None, z=None):
        if alph is None: return 0, matrix(1e-1, ( O.size[1],1))
        w = X * alph
        f =  - matrix(y).T*log(abs(w)) - matrix(1-y).T*log(abs(1-w))
        gr=[]
        for i in range(X.size[1]):
            gr.append(- matrix(y).T * div(X[:,i],w) + matrix(1-y).T* div((X[:,i]),(1-w)))
        grad = matrix(gr)
        if z is None: return f, grad.T
        hes = np.zeros((X.size[1], X.size[1]))
        for i in range(X.size[1]):
            for j in range(X.size[1]):
                tmp = X[:, i]
                for ii in range(X.size[0]):
                    tmp[ii] = -X[ii, i] * X[ii,j]
                hes[i,j] = (- matrix(y).T * div(tmp, w**2) - matrix(1-y).T * div((tmp), (1-w)**2))[0]
        H = matrix(hes, (X.size[1],X.size[1]))
        return f, grad.T, z[0]*H
    
    sol = solvers.cp(F, G=Gg, h=hh, A=Aa, b=bb)
    return sol['x']


# костыльный класс-обертка для того чтобы cross_val_predict - из sklean'a работал как cross_val_predict_proba
class BaseProbaEstimator(BaseEstimator):
    def __init__(self, estimator):
        self.estimator = estimator
        
    def fit(self, X, y):
        self.estimator.fit(X, y)
        return self
    
    def predict(self, X):
        return self.estimator.predict_proba(X)[:,1]
    
    def predict_proba(self, X):
        return self.estimator.predict_proba(X)[:,1]


class Blender(BaseEstimator):
    '''
    !NOTE: This stack-estimator only for binary classification ! 
    !NOTE: коэффиициенты получаем из минимизации log-Loss (кроссэнтропия)
    
    Example, how to use:
    blndr = Blender([XGB, SVC(with probas=True), RF, KNN])
    blndr.fit(X,y)
    blndr.predict_proba(X_test)
    
    # or with your weights:
    blndr = Blender([XGB, SVC(with probas=True), RF, KNN], with_own_weights = True)
    blndr.fit(X,y)
    w = np.array([0.5, 0.2, 0.2, 0.1])  #sum(w)==1
    blndr.predict_proba(X_test, weights = w)
    '''
    
    def __init__(self, estimators, with_own_weights = False):
        '''
        Parameters
        ----------
        estimators : array-like, shape = (n_estimators ,)
                     estimators is list of estimators(BaseEstimator)
        '''
        #assert
        self.estimators = estimators   
        self.labels_01 = True
        self.own_weights = with_own_weights
            
    def fit(self, X, y):
        '''
        Parameters
        ----------
        X : array-like, shape = [n_samples, n_features]
        
        y : array-like, shape (n_samples,)
            Target vector relative to X.
        '''
        skf = StratifiedKFold(y, n_folds=5, shuffle=True, random_state=0)
        self.skf = skf
        preds_= []
        for i in range(len(self.estimators)):
            preds_.append(cross_val_predict(BaseProbaEstimator(self.estimators[i]), X, y, cv=self.skf))
            self.estimators[i].fit(X,y)
        X = np.vstack(preds_)
        # запомним какие таргеты были -1 или 0 для отрицательного класса -- кривая реализация - но быстро
        if -1 in y:
            y = ((y+1)/2).astype(int)
            self.labels_01 = False
        if not self.own_weights:
            a = solver_cross_entr(X.T, y)    
            self.coef = a.T
        return self
    
    
    def predict(self, X, weights = None):
        '''
        Parameters
        ----------
        X : array-like, shape = [n_samples, n_features]
            
        Returns
        -------
        T : array-like, shape = (n_samples, )
            Returns the predicted values
        '''
        X = (self.predict_proba(X, weights)[:,1]>=0.5).astype(int)
        if not self.labels_01: # вернем обратно метки какие были;
            X = (X-1./2)*2
        return X
    
    
    def predict_proba(self, X, weights = None):
        '''
        Parameters
        ----------
        X : array-like, shape = [n_samples, n_features]
        
        Returns
        -------
        T : array-like, shape = [n_samples, n_classes]
            Returns the probabilities for each classes
        '''
        preds_= []
        for i in range(len(self.estimators)):
            preds_.append(self.estimators[i].predict_proba(X)[:,1])
        X = np.vstack(preds_)
        if not self.own_weights:
            X = (self.coef @ X)[0]
        else:
            assert np.sum(weights)==1.
            X = weights @ X
        X = np.vstack([1 - X, X]).T
        return X
    
    
    def grid_search(self):
        '''
        тут будет Grid_search
        '''
        return self