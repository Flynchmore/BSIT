#include <iostream>
using namespace std;

void playAgain();

int main()
{
    char playAgainInput;
    bool playAgainFlag = true;

    while (playAgain)
    {
        playAgain();
                cout << "\nDo you want to play again? (Y/N): ";
                cin >> playAgainInput;
            if(playAgainInput != 'Y' && playAgainInput != 'y'){
                playAgainFlag = false;
            }
                cout << "Thanks for playing!\n";
        }
}

void playAgain()
{
const int secretnum = 15;
const int maxAttempts = 10;
int attempts = 0;
int guess;

    cout << "Welcome to a Guessing Game!";
    cout << "\nYou have " << maxAttempts << " attempts only!\n";
    
    do
   {
        cout << "Guess a number from 1 to 25:  ";
            cin >> guess;
            attempts++;

    if (guess == secretnum)
    {
        cout << "\nCongrats! You have won! The secret number is: " << secretnum << "!";
        break;
    }
    else if (guess != secretnum)
    {
        cout << "Wrong Answer! You have only " << maxAttempts - attempts << " attempts left!\n";
    }
    if (attempts == maxAttempts)
    {
        cout << "Sorry! No more attempts left!\n";
    }
}while (attempts < maxAttempts);
}



