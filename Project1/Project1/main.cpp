#include <iostream>     // Include the iostream library
#include <fstream>      //required for working with files
#include <string>;      //required for strings
#include <functional>   //required for function
#include <ctime>        //required for time
#include <cstdlib>      //rand numbers
using namespace std;


//Function overloading
//Functions can have the same name as long as their parameters are different, this allows for this scenario
//Functions can also be overloaded using different amounts of variables, given they aren't set already

void println(string input = "", bool inFront = 1) { //default to empty string if none defined
    //function to imitate the println function in other languages
    std::cout << (inFront ? "\n" + input : input + "\n"); //put the newline either infront or behind based on variable
}
void println(int input = 0, bool inFront = 1) { //default to empty string if none defined
    //function to imitate the println function in other languages
    if (!inFront) { std::cout << input << "\n"; }
    else { std::cout << "\n" << input; }
}
void println(float input = 0.0, bool inFront = 1) { //default to empty string if none defined
    //function to imitate the println function in other languages
    if (!inFront) { std::cout << input << "\n"; }
    else { std::cout << "\n" << input; }

}

static string getDay(int index) {
    string days[7] = { "Monday", "Tuesday", "Wednesday", "Thursday", "Friday", "Saturday", "Sunday" };
    return days[index];
}

void printTest() {
    // Printing a string literal
    std::cout << "Hello, C++!";

    // Printing a variable
    int age = 30;
    std::cout << "My age is: " << age;

    // Printing with a newline character
    std::cout << "\nThis will be on a new line." << std::endl;
    std::cout << "Another line." << '\n'; // '\n' is often more efficient than std::endl

}

void structTest() {
    struct car{
        string brand;
        string model;
        int year;
    }; // We can add variables by separating them with a comma here

    // Put data into the first structure
    car myCar1;
    myCar1.brand = "BMW";
    myCar1.model = "X5";
    myCar1.year = 1999;

    // Put data into the second structure
    car myCar2;
    myCar2.brand = "Ford";
    myCar2.model = "Mustang";
    myCar2.year = 1969;

    // Print the structure members
    std::cout << myCar1.brand << " " << myCar1.model << " " << myCar1.year << "\n";
    std::cout << myCar2.brand << " " << myCar2.model << " " << myCar2.year << "\n";
}
void ifStatements() {
    bool a = false;
    bool b = true;

    if (a) {
        std::cout << "\nI will never run";
    } 
    else if (!a) {
        std::cout << "\nI will run";
    }
    else if (!a && b) {
        std::cout << "\n And Statement";
    }
    else if (a || b) {
        std::cout << "\n Or Statement";
    }
    else {
        std::cout << "I will never run";
    }
    int i = 23;
    std::cout << "\n" << ((i > 22) ? "Bigger" : "Smaller"); // ? symbol defines a true false if statement for boolean values
    i = 21;
    std::cout << "\n" << ((i > 22) ? "Bigger" : "Smaller");
    std::cout << "\n" << (b ? "True Value" : "False Value"); // works with a boolean variable too
}
void enumerator() {
    enum LEVEL { //assign names to constants
        LOW = 10,
        MEDIUM = 75,
        HIGH = 100,
        OVERLOAD = 120
    };

    enum LEVEL a = MEDIUM;
    std::cout << "\n" << a;


    enum constants {    //assigning a value to one of the constants will make the rest index based around it
        con1 = 1,       //value of 1, obviously
        con2,           //value of 2
        con3            //value of 3
    };
    enum constants b = con1;
    std::cout << "\n";

    switch (b) { //switch statements can be used to trigger events without writing if statements
        case 1:                         //if (value of enum is 1)
            std::cout << "Low Level";
            break;
        case 2:                         //else if (value of enum is 2)
            std::cout << "Medium level";
            break;
        case 3:                         //else if (value of enum is 3)
            std::cout << "High level";
            break;
    }
}

void memory() {
    string food = "Pizza"; // food variable
    string& meal = food;   // reference to food
    
    meal = "Burger"; // changes both meal and food

    println(food);
    println(meal);

    string* pointerFood = &food; //pointer to food(gets memory address of food)
    std::cout << "\n" << pointerFood;   //pointer holds address of food
    std::cout << '\n' << *pointerFood;  //get value at address

    *pointerFood = "Hamburger";         //changing pointer value will change main variable value
    println(food);

    println("", 0);
    int* ptr = new int; //create new variable and store position in ptr
    *ptr = 35;          //set value at pointer to 35
    cout << "\n" << *ptr;

    delete ptr;         //release memory
}

void files() {
    // Create and open a text file
    //ifstream reads only, ofstream creates and writes only
    //fstream does both

    //write to file
    ofstream MyFile("filename.txt");
    MyFile << "Files can be tricky, \nbut it is fun enough!";
    MyFile.close();  //close file

    //read from file
    ifstream MyFile2("filename.txt");
    string output;
    while (getline(MyFile2, output)) { println(output); } //print file output
    MyFile2.close(); //close file
}


void changeValue(int &num, int num2) {    //pass variable into function using &, otherwise they will be local
    num = 50;
    num2 = 60;
}

int sum(int k) {
    if (k > 0) {
        return k + sum(k - 1);  //calls itself recursively 
                                //10+9+8+7...+1
    }
    else {
        return 0;
    }
}

void runTwice(function<void()> func) {
    func();
    func();
}




class berries {
public:
    int age;
    string type;
    float unchangedOnCreation = 1.1;

    //constructor, defines parameters for function
    //constructors can be overloaded too
    berries() {
        type = "RASPBERRY";
        age = 0;
    }
    berries(string x, int y) {
        type = x;
        age = y;
    }
    friend void getMessage(berries berr);  //specify friend function
private:    //obj.whatever wouldn't be able to access this
            //protected: specifier allows access from inherited classes too
    string access = "Only accessible from within function";
    //if no access specifier is listed, it defaults to private
};

void getMessage(berries berr) {  //friend functions can access data within a class
    println(berr.access);
}

//inheritance
//inherited classes keep everything public or protected from their parents, but it can be ignored/overwritten
class berry : public berries {
public:
    int amount = 1;
};


//templates
//templates can use a placeholder instead of datatypes
template <typename T>
T tadd(T a, T b) {
    return a + b;
}





void main() {
    printTest();
    println("");

    structTest();
    println("");

    println(getDay(1), 0);

    ifStatements();
    println("");

    enumerator();
    println("");

    memory();
    println("");

    files();
    println("");
    println("");

    int x = 35;
    int y = 40;
    changeValue(x, y);
    std::cout << x << ", " << y;
    println("");

    println(sum(10));
    println("");
    
    //Lambda 
    //auto name = [](parameters) { function };
    auto add = [](int a, int b) {
        return a + b;
        };
    println(add(3, 4));

    auto message = []() {
        println("Hello");
        };
    runTwice(message);
    println("");

    berries obj("STRAWBERRY", 2);
    println(obj.type);
    println(obj.unchangedOnCreation);
    berries obj2;
    println(obj2.type);
    getMessage(obj2);
    println("");

    berry b;
    println(b.amount);

    //define the datatype of the template
    println(tadd<int>(1, 2));
    println("");

    //random numbers
    srand(time(0));             //seed is time
    println(rand() % 1001);
    println(rand() % 1001);
}