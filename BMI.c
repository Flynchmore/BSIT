#include <stdio.h>

int main(){

    int Constant;
    float Weight, Height, BMI;
    
    Constant = 10000;
        printf("Insert your weight in kilograms: ");
        scanf("%f", &Weight);
        printf("Insert your height in meters: ");
        scanf("%f", &Height);
        
        BMI = Weight / (Height * Height) * Constant;
        
        printf("You're BMI is: %.2f\n", BMI);
        
        if(BMI < 18.5){
            printf("You're underweight!");
        }else if((BMI == 18.5) || (BMI <= 24.9)){
            printf("You're in normal weight!");
        } else if((BMI == 25) || (BMI <= 29.9)){
            printf("You're overweight!");
        }else if(BMI >= 30){
            printf("You're obesity!");
        }else
            printf("Error");
return 0;
}