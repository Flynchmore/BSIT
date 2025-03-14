#include <stdio.h>
int main(){
  
    char choi, col;
    float N1, N2, Total;
    
        printf("1. Color\n");
        printf("2. Add Two Numbers\n");
        printf("Insert your choice: ");
            scanf("%c", &choi);
            
            if(choi == '1'){
                printf("Insert a color: ");
                   scanf(" %c", &col);
               if(col == 'Y' || col == 'y'){
                   printf("Yellow");
          }        
                  else
                      printf("The %c is an invalid Color!");
            }
            else if(choi == '2'){
                      printf("Insert a First Number: ");
                          scanf("%f", &N1);
                      printf("Insert the Second Number: ");
                          scanf("%f", &N2);
                          
                          Total = N1 + N2;
                              printf("The total is: %.2f", Total);
                             }  
                             else
                                 printf(" The %c is an invalid Choice!");
                            
                             
    return 0;
}