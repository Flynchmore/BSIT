#include <iostream>
#include <iomanip>

using namespace std;

int main(){

    double Q1, Q2, A1, A2, A3, A4, ASS1, ASS2, MAJEX, QA, AA, ASSA, MAJEXA, FINAL;

    cout << "Insert Quiz1: ";
        cin >> Q1;
    cout << "Insert Quiz2: ";
        cin >> Q2;
    cout << "Insert Activity1: ";
        cin >> A1;
    cout << "Insert Activity2: ";
        cin >> A2;
    cout << "Insert Activity3: ";
        cin >> A3;
    cout << "Insert Activity4: ";
        cin >> A4;
    cout << "Insert Assignment1: ";
        cin >> ASS1;
    cout << "Insert Assignment2: ";
        cin >> ASS2;
    cout << "Insert Major Exam Score: ";
        cin >> MAJEX;

    cout << "\n";

    QA = (Q1 + Q2) / 2 * 0.15;
    AA = (A1 + A2 + A3 + A4) / 4 * 0.10;
    ASSA = (ASS1 + ASS2) / 2 * 0.35;
    MAJEXA = MAJEX * 0.40;
    FINAL = QA + AA + ASSA + MAJEXA;

    if (FINAL >= 75){
        cout << "You are passed with an average of: " << fixed << setprecision(2) << FINAL <<endl;
    }
    else    
        cout << "You are failed with an average of: " << fixed << setprecision(2) << FINAL <<endl;

    cout <<"\n";
    
    cout << "Quiz = " << QA << "%" <<endl;
    cout << "Acitivity = " << AA << "%" <<endl;
    cout << "Assignment = " << ASSA << "%" <<endl;
    cout << "Major Exam = " << MAJEXA << "%" <<endl;

    return 0;
}