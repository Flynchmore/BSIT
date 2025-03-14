#include <iostream>
#include <iomanip>

int main() {
    const int NUM_QUIZZES = 2;
    const int NUM_ACTIVITIES = 4;
    const int NUM_ASSIGNMENTS = 2;
    const int NUM_EXAMS = 1;
    
    double quizzes[NUM_QUIZZES];
    double activities[NUM_ACTIVITIES];
    double assignments[NUM_ASSIGNMENTS];
    double exams[NUM_EXAMS];

    // Input grades
    std::cout << "Enter grades for " << NUM_QUIZZES << " quizzes:\n";
    for (int i = 0; i < NUM_QUIZZES; ++i) {
        std::cout << "Quiz " << i + 1 << ": ";
        std::cin >> quizzes[i];
    }
    
    std::cout << "Enter grades for " << NUM_ACTIVITIES << " activities:\n";
    for (int i = 0; i < NUM_ACTIVITIES; ++i) {
        std::cout << "Activity " << i + 1 << ": ";
        std::cin >> activities[i];
    }
    
    std::cout << "Enter grades for " << NUM_ASSIGNMENTS << " assignments:\n";
    for (int i = 0; i < NUM_ASSIGNMENTS; ++i) {
        std::cout << "Assignment " << i + 1 << ": ";
        std::cin >> assignments[i];
    }
    
    std::cout << "Enter grade for the Major Exam: ";
    std::cin >> exams[0];

    // Calculate weighted averages
    double total_weight = 0.15 * NUM_QUIZZES + 0.10 * NUM_ACTIVITIES + 0.35 * NUM_ASSIGNMENTS + 0.40 * NUM_EXAMS;
    double weighted_sum = 0;

    for (int i = 0; i < NUM_QUIZZES; ++i) {
        weighted_sum += quizzes[i] * 0.15;
    }
    for (int i = 0; i < NUM_ACTIVITIES; ++i) {
        weighted_sum += activities[i] * 0.10;
    }
    for (int i = 0; i < NUM_ASSIGNMENTS; ++i) {
        weighted_sum += assignments[i] * 0.35;
    }
    weighted_sum += exams[0] * 0.40;

    double average = weighted_sum / total_weight;

    std::cout << "\nYou are passed with an average of: " << std::fixed << std::setprecision(2) << average << "%" << std::endl;

    return 0;
}
