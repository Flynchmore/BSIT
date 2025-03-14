#include <iostream>
#include <string>
#include <chrono>
#include <ctime>

using namespace std;
using namespace std::chrono;

int main(){
    string choice, studname, studid, course, ssubjectcode, sched;
    string instructname, isubjectcode, classsched;
    time_t now = system_clock::to_time_t(system_clock::now());
    
    cout << "Attendance Tracking System" << endl;

    while (true) {
        cout << "\nStudent or Instructor: ";
        cin >> choice;
        cin.ignore();

        if (choice == "Student" || choice == "student") {
            cout << "Student Name: ";
            getline(cin, studname);
            cout << "Student ID: ";
            getline(cin, studid);
            cout << "Course: ";
            getline(cin, course);
            cout << "Subject Code: ";
            getline(cin, ssubjectcode);
            cout << "Time In: " << ctime(&now) << endl;
            break; // Exit loop after successful student input
        } else if (choice == "Instructor" || choice == "instructor") {
            cout << "Instructor Name: ";
            getline(cin, instructname);
            cout << "Subject Code: ";
            getline(cin, isubjectcode);
            cout << "Class Schedule: ";
            getline(cin, classsched);
            break; // Exit loop after successful instructor input
        } else {
            cout << "Incorrect Input! Please enter 'Student' or 'Instructor'." << endl;
        }
    }

    return 0;
}
