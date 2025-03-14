#include <stdio.h>
int main(){
    
     int Q1, Q2, Q3, Q4;
     float FA;
     
         printf("Insert for Quiz 1: ");
             scanf("%d", &Q1);
         printf("Insert for Quiz 2: ");
             scanf("%d", &Q2);
         printf("Insert for Quiz 3: ");
             scanf("%d", &Q3);
         printf("Insert for Quiz 4: ");
             scanf("%d", &Q4);
      printf("\n");
      
          FA = (Q1 + Q2 + Q3 + Q4) / 4;
          
      if(FA == 100){
          printf("Your Final Grade is: %.2f", FA);
          printf("\nA");
      }
      else if(FA <= 100 && FA >= 90){
          printf("Your Final Grade is: %.2f", FA);
          printf("\nB");
      }
      else if(FA <= 89 && FA >= 80){
          printf("Your Final Grade is: %.2f", FA);
          printf("\nC");
      }
      else if(FA >= 79 && FA <= 75){
          printf("Your Final Grade is: %2.f", FA);
          printf("\nD");
      }
     else if(FA >= 0 && FA <= 74){
          printf("Your Final Grade is: %2.f", FA);
          printf("\nE");
     }
       else
          printf("Invalid Grade");
       
       return 0;
}