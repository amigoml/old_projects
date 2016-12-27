import mysql.connector
import datetime
from datetime import datetime
import csv
import sys

index = sys.argv[1]

config = {
  'user': 'root',
  'password': 'root',
  'unix_socket': '/Applications/MAMP/tmp/mysql/mysql.sock',
  'database': 'tmp_bd',
  'raise_on_warnings': True,
}

def test_gogovyvod():
	link = mysql.connector.connect(**config)
	cursor = link.cursor()
	cursor.execute("SELECT * FROM table1")
	row = cursor.fetchone()
	while row is not None:
		print(row)
		row = cursor.fetchone()
	cursor.close()
	link.close()

###-------------------
def read_and_incert_csv():
    insert_record(read_data())

def give_splitted(x):
	res=datetime.strptime(x[1], '%Y-%m-%d %H:%M:%S')
	return (int(x[0]),res)

def read_data():
	with open('./payment_log.csv', 'rb') as f:
		reader = csv.reader(f)
		a=list(reader)
		b=map(tuple,a)[1:]
		c=map(give_splitted,b)
	return c
####--------------------


###---------------------
def insert_record(rec):
    query = "INSERT INTO table1(user_id,dt) " \
            "VALUES(%s,%s)"
    link = None
    cursor=None
    try:
		link = mysql.connector.connect(**config)

		cursor = link.cursor()
		cursor.executemany(query, rec)
		link.commit()

    finally:
        cursor.close()
        link.close()
###----------------------




def sql_query(num=1):
	if num in [1,2,3,4]:
		link = mysql.connector.connect(**config)
		cursor = link.cursor()
		cursor.execute(queries[num-1])
		row = cursor.fetchone()
		while row is not None:
			print(row)
			row = cursor.fetchone()
		cursor.close()
		link.close()


queries=[''' 
SELECT  min(month(dt)), user_id
FROM `table1`
GROUP by (user_id)
ORDER by month(dt)
''','''
SELECT  DISTINCT Month(t.dt), t.user_id
FROM `table1` as t,`table1` as d
where (Month(t.dt) = (Month(d.dt)+1)) and (t.user_id = d.user_id)
''','''
SELECT  DISTINCT Month(t.dt), t.user_id
FROM `table1` as t,`table1` as d
where (Month(t.dt) > (Month(d.dt)+1)) and (t.user_id = d.user_id)
''','''
SELECT Month(t.dt)+1,t.user_id
FROM `table1` as t
where  Month(t.dt)+1 not in (
SELECT Month(dt)
FROM `table1`
where t.user_id=user_id
)
''']



# reading from csv and recording in db

# read_and_incert_csv()

# test_gogovyvod()


if (int(index)==0):
	read_and_incert_csv()
	test_gogovyvod()
else:
	sql_query(int(index))





