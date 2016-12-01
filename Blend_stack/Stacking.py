import numpy as np
from sklearn.cross_validation import cross_val_predict, StratifiedKFold
from sklearn.base import BaseEstimator

# костыльный класс-обертка для того чтобы cross_val_predict - из sklean'a работал как cross_val_predict_proba
class BaseProbaEstimator(BaseEstimator):
    def __init__(self, estimator):
        self.estimator = estimator
        
    def fit(self, X, y):
        self.estimator.fit(X, y)
        return self
    
    def predict(self, X):
        return self.estimator.predict_proba(X)
    
    def predict_proba(self, X):
        return self.estimator.predict_proba(X)

class Stacker(BaseEstimator):
    '''
    !NOTE: This stack-estimator only for binary classification because of predict(self, X) function returns ! 
    
    (Инициализация идет с помощью списка слоев. Каждый слой - список классификаторов.)
    Example, how to use:
    stkr = Stacker([ [XGB, SVC(with probas=True)],[RF, KNN],[LR] ])
    # init with three layer, on last Logistic Regression.
    stkr.fit(X,y)
    stkr.predict_proba(X_test) 
    '''
    def __init__(self, estimators):
        '''
        Parameters
        ----------
        estimators : array-like, shape = [n_layers, {n_estimators (diff for each layer)}]
        '''
        assert len(estimators[-1])==1 # проверим что на последнем уровне один классификатор
        #assert
        #assert
        self.estimators = estimators   
            
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
        
        for n_level in range(len(self.estimators)):
            preds_= []
            for i in range(len(self.estimators[n_level])):
                preds_.append(cross_val_predict(BaseProbaEstimator(self.estimators[n_level][i]), X, y, cv=self.skf))
                self.estimators[n_level][i].fit(X,y)
            X = np.hstack(preds_)
            
        return self
    
    def predict(self, X):
        '''
        Parameters
        ----------
        X : array-like, shape = [n_samples, n_features]
            
        Returns
        -------
        T : array-like, shape = [n_samples, n_classes]
            Returns the predicted values
        '''
        X = self.predict_proba(X)
        X = X[:,1]
        return (X>=0.5).astype(int)
    
    def predict_proba(self, X):
        '''
        Parameters
        ----------
        X : array-like, shape = [n_samples, n_features]
            
        Returns
        -------
        T : array-like, shape = [n_samples, n_classes]
            Returns the probability of the sample for each class in the model,
            where classes are ordered as they are in estimator from last layer, 
            ?? but its I think identically for all layers (if it's inherited from BaseEstimator) ??
        '''
        for n_level in range(len(self.estimators)):
            preds_= []
            for i in range(len(self.estimators[n_level])):
                preds_.append(self.estimators[n_level][i].predict_proba(X))
            X = np.hstack(preds_)
        return X
    
    def grid_search(self):
        '''
        Grid_search? 
        '''
        return self