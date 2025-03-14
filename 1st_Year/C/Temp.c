#include<stdio.h>

int main()
{
    float C_Value, F_Value, Formula_Celsius, Formula_Fahrenheit;
    char Temp;
    
    Temp =='C', 'F';
    
    
    printf("Select a temperature that you wanted to convert: ");
    scanf("%c", &Temp);
    
    
    //If the user wanted to Convert an Fahrenheit into Celsius
    if(Temp == 'C')
    {
       printf("Please insert a value of Fahrenheit to convert: ");
       scanf("%f", &F_Value);
         
          Formula_Celsius = (F_Value - 32) * 5/9;
           printf("The value has been converted into: %.2f°C", Formula_Celsius);
    }
    
    //If the user wanted to Convert a Celsius into Fahrenheit
    else if(Temp == 'F')
    {
        printf("Please insert a value of Celsius to convert: ", Formula_Fahrenheit);
        scanf("%f", &C_Value);
          
           Formula_Fahrenheit = (C_Value * 9/5) + 32;
            printf("The value has been converted into: %.2f°F", Formula_Fahrenheit);
    }
    
    else
        printf("%c Oops, wrong Temperature!");
    return 0;
}