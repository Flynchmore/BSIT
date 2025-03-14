#include<stdio.h>

int main()
{
    float Num1, Num2, Total;
    char Ope = ('+', '-', '/', '*');
    
         printf("Choose an Operator: ");
         scanf("%c", &Ope);
         
         printf("Insert a Value for 1st Number: ");
         scanf("%f", &Num1);
         
         printf("Insert a value for 2nd Number: ");
         scanf("%f", &Num2);
         
    switch(Ope)
    {    case '+': 
           
       Total = Num1 + Num2;
            printf("The sum is %.2f", Total);
        break;
        
        case '-':
           
       Total = Num1 - Num2;
            printf("The difference is %.2f", Total);
        break;
        
        case '/':
           
       Total = Num1 / Num2;
            printf("The quotient is %.2f", Total);
        break;
        
        case '*':
            
       Total = Num1 * Num2;
       
            printf("The product is %.2f", Total);
        break;
        
        default:
            printf("%c Invalid Operator!");
            
    }
    return 0;
}