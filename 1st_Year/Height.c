#include<stdio.h>
int main(){
    
    float hei;
    
        printf("Insert your Height: ");
            scanf("%f", &hei);
        
        if(hei >= 0.5 && hei <= 1){
            printf("Dwarf");
        }else if(hei >= 1.1 && hei <= 2){
            printf("Hobbit");
        }else if(hei >= 2.1 && hei <= 3){
            printf("Elf");
        }else if(hei >= 3.1 && hei <= 4){
            printf("Human");
        }else if(hei >= 4.1 && hei <= 5){
            printf("Giant");
        }
        else 
            printf("Invalid Height");
    
    return 0;
}