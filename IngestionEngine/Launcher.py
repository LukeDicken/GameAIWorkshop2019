import falcon, sqlite3

class Status:

    def on_get(self, req, resp):
        resp.body = "HELLO WORLD"

class CustomEvent:

    pass

class SessionStart:

    pass

api = application = falcon.API()
api.add_route('/status', Status())