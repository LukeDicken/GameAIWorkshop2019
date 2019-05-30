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

class CounterEvent:
    def __init__(self, db):
        self.db = db


    def on_post(self, req, resp):
        # get the PlayerID, current time
        # counterName primaryParameter and sessionID
        # put the rest of the passed data into a dictionary
        # insert into appropriate rows in counters table
        # generate a JSON status=ok
        pass

class SessionStart:

    def __init__(self, db):
        self.db = db

    def on_post(self, req, resp):
        # retrieve the PlayerID, startTime and SessionID from passed parameters
        # annotate with the current time
        # insert into sessions
        # return a JSON status=ok blob
        pass

class PlayerRegisterEvent:

    def __init__(self, db):
        self.db = db

    def on_post(self, req, resp):
        # retrieve the PlayerID
        # get the current time
        # insert the data into the players table
        # return a json whose status is ok
        # consider try/excepting to return status="fail" as appropriate
        pass


class PlayerType:
    def __init__(self, db):
        self.db = db

    def on_get(self, req, resp):
        # get the PlayerID from the req object
        # create a SQL statement to count how many sessions that PlayerID has
        # execute and retrieve the value
        # is the number even or odd?
        # return the result as a JSON payload on the configValue key
        pass

def sql_setup():
    sql = sqlite3.Connection(database='data/Datastore.sql')
    try:
        playerSetup = "CREATE TABLE IF NOT EXISTS players (installTime timestamp, playerID varchar(50))"
        sessionSetup = "CREATE TABLE IF NOT EXISTS sessions (statTime timestamp, sessionStartTime timestamp, playerID varchar(50), sessionID varchar(50))"
        counterSetup = "CREATE TABLE  IF NOT EXISTS counters (statTime timestamp, sessionID varchar(50), playerID varchar(50), counterName varchar(50), primaryParameter varchar(50), parameterBlob varchar(250))"
        sql.execute(playerSetup)
        sql.execute(sessionSetup)
        sql.execute(counterSetup)
    except sqlite3.OperationalError as error:
        print(error)
        print("Tables are (probably) already created")
    return sql

sql = sql_setup()
api = application = falcon.API()
api.add_route('/status', Status(sql))



api.add_route('/newPlayer', PlayerRegisterEvent(sql))
api.add_route('/sessionStart', SessionStart(sql))
api.add_route('/counter', CounterEvent(sql))
api.add_route('/playerType', PlayerType(sql))