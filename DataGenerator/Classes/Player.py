from datetime import datetime as dt


class Player:

    platform = ""
    install_date = ""
    install_version = ""
    install_date = ""
    levelMax = 0

    def __init__(self, playerID, startDate):
        self.playerID = playerID
        self.install_date = dt.fromtimestamp(startDate)
        # figure out a way of factoring a retention curve in here

    def getEndDate(self):
        # use a retention curve to figure out what your end date was
        pass

    def generateSessions(self):
        # generate session data between the startDate and endDate
        pass

    def generateSingleSession(self):
        # inside a single session
        # determine
        pass

    def generateLevelAttempt(self):
        # player might attempt an old level
        # or they might attempt the next level
        pass

    def __str__(self):
        return("Player " + str(self.playerID) + "\n\t" + dt.strftime(self.install_date, '%d-%m-%Y %H:%M:%S'))
