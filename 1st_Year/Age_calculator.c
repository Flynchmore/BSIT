#include <stdio.h>

int main(){

    int Birth_year, Your_age;

        printf("Insert your Birth Year: ");
        scanf("%d", &Birth_year);

            Your_age = 2023 - Birth_year;

            if(Your_age >= 18){
                printf("You're %d years old!\n");
                printf("You're an adult!");
            }
            else if(Your_age != 18){
                printf("You're %d years old!\n");
                printf("Sadly, you're a minor!");
            }
            else
                printf("Error!");

            
    return 0;
}