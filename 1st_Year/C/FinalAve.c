#include <stdio.h>

int main()

{
 float Q1, Q2, A1, R1, R2, A2, FA;
 
     printf("Input Quiz 1: ");
         scanf("%f", &Q1);
     printf("Input Quiz 2: ");
         scanf("%f", &Q2);
     printf("Input Recitation 1: ");
         scanf("%f", &R1);
     printf("Input Recitation 2: ");
         scanf("%f", &R2);
         
        A1 = (Q1 + Q2) / 2;
            printf("\nThe average for Quiz is: %.2f", A1);
        A2 = (R1 + R2) / 2;
            printf("\nThe average for Recitation is: %.2f", A2);
            
        FA = A1 + A2;
            printf("\nThe Final average is: %.2f", FA);
     
 return 0;
}