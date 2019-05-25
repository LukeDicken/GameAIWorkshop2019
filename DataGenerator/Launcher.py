from DataGenerator.Classes.Player import Player
import random


def run_simulation(playerCount, startDate, endDate):
    # instantiate the required number of players
    # pick a distribution of install dates between startDate and endDate
    players = []
    for i in range(0, playerCount):
        install = random.randint(startDate, endDate)
        players.append(Player(i, install))
    for player in players:
        print(player)

if __name__ == "__main__":
    # flip these parameters to be CLI driven
    playerCount = 10
    startDate = 1544000000
    endDate = 1558000000
    run_simulation(playerCount,startDate,endDate)