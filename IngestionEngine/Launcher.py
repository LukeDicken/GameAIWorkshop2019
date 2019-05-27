import sqlite3, pandas as pd

class Status:

    def on_get(self, req, resp):
        resp.body = "HELLO WORLD"

class CustomEvent:

    pass

class SessionStart:

    pass

# api = application = falcon.API()
# api.add_route('/status', Status())

def sql_play():
    sql = sqlite3.Connection(database='data/hello.sql')
    # try:
    #     sql.execute("CREATE TABLE test1 (stat_date timestamp, player_id int, metric varchar(50), payload varchar(100))")
    # except sqlite3.OperationalError:
    #     print("Table already exists")
    #sql.execute("INSERT INTO test1 (player_id, metric, payload) VALUES (5, 'hello', 'world')")
    #sql.commit()
    thing = sql.execute("SELECT * FROM test1")
    df = pd.DataFrame(thing)
    print(df)




if __name__ == "__main__":
    sql_play()