#include<stdio.h>

int main()
{
    float N1_V, N2_V, Add_F, Sub_F, Mult_F, Div_F;
    char Ope;
    
    Ope =='+', '-', '*', '/';
    
    
    printf("Select an operator: ");
    scanf("%c", &Ope);
    
    
    //If the user wanted to add
    if(Ope == '+')
    {
       printf("Please insert a value for 1st Integer: ");
       scanf("%f", &N1_V);
       printf("Please insert a value for 2nd Integer: ");
       scanf("%f", &N2_V);  
          
          Add_F = N1_V + N2_V;
           printf("The sum of %.2f and %.2f is: %.2f", N1_V, N2_V, Add_F);
    }
    
    //If the user wanted to subtract
    else if(Ope == '-')
    {
       printf("Please insert a value for 1st Integer: ");
       scanf("%f", &N1_V);
       printf("Please insert a value for 2nd Integer: ");
       scanf("%f", &N2_V);  
           
          Sub_F = N1_V - N2_V;
           printf("The difference of %.2f and %.2f is: %.2f", N1_V, N2_V, Sub_F);
    }
   
   //If the user wanted to multiply
   else if(Ope == '*')
    {
       printf("Please insert a value for 1st Integer: ");
       scanf("%f", &N1_V);
       printf("Please insert a value for 2nd Integer: ");
       scanf("%f", &N2_V);  
           
          Mult_F = N1_V * N2_V;
           printf("The product of %.2f and %.2f is: %.2f", N1_V, N2_V, Mult_F);
    }
  
   //If the user wanted to divide
   else if(Ope == '/')
    {
       printf("Please insert a value for 1st Integer: ");
       scanf("%f", &N1_V);
       printf("Please insert a value for 2nd Integer: ");
       scanf("%f", &N2_V);  
           
          Div_F = N1_V / N2_V;
           printf("The quotient of %.2f and %.2f is: %.2f", N1_V, N2_V, Div_F);
    }
    
    else
        printf("%c Oops, Invalid Operator!");
    return 0;
}