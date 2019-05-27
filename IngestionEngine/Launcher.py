import falcon, sqlite3, pandas as pd, json, numpy as np
import datetime as dt

class Status:


    def __init__(self, db):
        self.db = db

    def on_get(self, req, resp):
        data = {}
        cursor = self.db.execute("SELECT COUNT(*) FROM players")
        df = pd.DataFrame(cursor)
        data['PlayerCount'] = (int)((df.iloc[0][0]).astype(np.int32))
        cursor = self.db.execute("SELECT COUNT(*) FROM sessions")
        df = pd.DataFrame(cursor)
        data['SessionCount'] = (int)((df.iloc[0][0]).astype(np.int32))
        cursor = self.db.execute("SELECT COUNT(*) FROM counters")
        df = pd.DataFrame(cursor)
        data['EventCount'] = (int)((df.iloc[0][0]).astype(np.int32))
        resp.body = json.dumps(data)

class CustomEvent:
    def __init__(self, db):
        self.db = db

    # how do we represent a custom event?
    def on_post(self, req, resp):
        pass

class SessionStart:

    def __init__(self, db):
        self.db = db

    def on_post(self, req, resp):
        pass

    # Determine the taxonomy for a session event
    pass

class PlayerRegisterEvent:

    def __init__(self, db):
        self.db = db

    def on_post(self, req, resp):
        playerID = req.media['PlayerID']
        now = dt.datetime.now()
        insertStatement = "INSERT INTO players (installTime, playerID) VALUES ('" + str(now) + "', '" + str(playerID) + "')"
        print(insertStatement)
        self.db.execute(insertStatement)
        data = {"status":"ok"}
        resp.body = json.dumps(data)


class PlayerType:
    def __init__(self, db):
        self.db = db

    def on_post(self, req, resp):
        pass


def sql_setup():
    sql = sqlite3.Connection(database='data/Datastore.sql')
    try:
        playerSetup = "CREATE TABLE players (installTime timestamp, playerID varchar(50))"
        sessionSetup = "CREATE TABLE sessions (sessionTime timestamp, playerID varchar(50))"
        counterSetup = "CREATE TABLE counters (statTime timestamp, playerID varchar(50), counterName varchar(50), parameterBlob varchar(250))"
        sql.execute(playerSetup)
        sql.execute(sessionSetup)
        sql.execute(counterSetup)
    except sqlite3.OperationalError:
        print("Tables are (probably) already created")
    return sql

sql = sql_setup()
api = application = falcon.API()
api.add_route('/status', Status(sql))
api.add_route('/newPlayer', PlayerRegisterEvent(sql))