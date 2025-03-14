#include<stdio.h>

int main()
{
    float Km_Value, M_Value, Constant, Formula_Meter, Formula_Kilometer;
    char Con_val;
    
    Constant = 1000.0;
    Con_val =='M', 'K';
    
    
    printf("Select a measurement that you wanted to convert: ");
    scanf("%c", &Con_val);
    
    
    //If the user wanted to Convert an Kilometer into Meter
    if(Con_val == 'M')
    {
       printf("Please insert a value of Kilometer to convert: ");
       scanf("%f", &Km_Value);
         
          Formula_Meter = Km_Value * Constant;
           printf("The value has been converted into: %.2f Meter", Formula_Meter);
    }
    
    //If the user wanted to Convert a Meter into Kilometer
    else if(Con_val == 'K')
    {
        printf("Please insert a value of Meter to convert: ", Formula_Kilometer);
        scanf("%f", &M_Value);
          
           Formula_Kilometer = M_Value / Constant;
            printf("The value has been converted into: %.2f Kilometer", Formula_Kilometer);
    }
    
    else
        printf("%c Oops, wrong measurement!");
    return 0;
}