# Demo-Strategy-Game
## General Design
- Will be added...
## Concepts
- **Object Oriented Programming**
  - Implemented with OOP Language (C#)
- **Polymorphism**
  - Abstract class
    - Example - ProductionButton class
- **S-O-L-I-D**
  - Single Responsibility Principle (SRP)
    - Example - AStarPathFinding2D class has one purpose
  - Open Closed Principle (OCP)
    - Example - ProductionButton class can be used for new button types
  - Liskov Substitution Principle (LSP)
    - Example - ShowBuildArea() function in ProductionButton class is implemented as abstract. Therefore, differently sized units derived from this class can implement their own build area.
  - Interface Segregation Principle (ISP)
    - There is no known class which is forced to implement function that it doesn't use.
  - Dependency Inversion Principle (DIP)
    - In my design there is no high-level module (except abstract class) in my design. Therefore, there is no need to consider DIP principle
# TO BE CONTINUED...
