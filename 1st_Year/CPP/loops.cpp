#include <iostream>
#include <cstdlib>
using namespace std;

void playGame();
int main() {
    char playAgainInput;
    bool playAgain = true;

    while (playAgain) {
        playGame();

        cout << "Do you want to play again? (y/n): ";
        cin >> playAgainInput;
        if (playAgainInput != 'y' && playAgainInput != 'Y') {
            playAgain = false;
        }
    }

    return 0;
}

void playGame() {
    const int secretNumber = 15;
    int guess;
    int attempts = 0;
    const int maxAttempts = 10; // Change this to set the maximum attempts allowed
    const int attemptsThreshold = 1; // Change this to set the attempts threshold for encouragement

    srand(static_cast<unsigned int>(time(0))); // Seed the random number generator

    cout << "Welcome to the Guessing Game!\n";
    cout << "You have " << maxAttempts << " attempts to guess the secret number.\n";

    do {
        cout << "Enter your guess: ";
        cin >> guess;
        attempts++;

        if (guess == secretNumber) {
            cout << "Congratulations! You guessed the correct number!\n";
            break;
        } else {
            if (attempts == maxAttempts - attemptsThreshold) {
                const char* encouragement[] = {"Come on, you can do this!", "Keep going!", "Almost there!"};
                cout << "You have " << maxAttempts - attempts << " attempts left. " << encouragement[rand() % 3] << endl;
            } else if (attempts == maxAttempts) {
                cout << "Sorry, you've run out of attempts. The secret number was " << secretNumber << ".\n";
            } else {
                cout << "Wrong guess. You have " << maxAttempts - attempts << " attempts left.\n";
            }
        }
    } while (attempts < maxAttempts);
}