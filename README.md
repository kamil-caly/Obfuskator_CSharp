# Inżynieria pozyskiwania i ochrony wiedzy z danych i baz danych - C# Code Obfuscator
- Kamil Cały
- Dominik Polak

## Introduction
Code Obfuscator is an application designed to enhance the security of C# applications by employing code obfuscation techniques. Code obfuscation involves making the source code more challenging to understand and analyze while preserving its functionality. This tool is particularly useful in safeguarding applications against unauthorized access, reverse engineering, and intellectual property theft.

## Implementation
The project is developed using Visual Studio 2022 and utilizes the Roslyn library for source code analysis. The application is divided into three main projects:

1. **Project_Tests:** A console program for testing various solutions related to source code analysis.

2. **Project_Logic:**
   - **Rewriters:** Classes and methods for code obfuscation using the Roslyn library.
   - **Entities:** Entity classes for storing information about compilation and code execution errors.
   - **CodeExecution:** Classes for code compilation and execution.
   - **Obfuscators:** Classes for word encryption and overall code obfuscation.

3. **Project_Gui:** A WPF-based graphical user interface for interacting with the application. The interface allows users to input or load C# code, obfuscate it, and view the compilation and execution results.

## Usage
1. **Code Input:**
   - Paste or write C# code in the editing panel.
   - Load C# code from a .txt file using the provided button.

2. **Code Obfuscation:**
   - Click the obfuscation button to obfuscate the code.

3. **Compilation and Execution:**
   - Press the button to compile and run the original and obfuscated code.
   - View the results in the respective panels.

4. **Additional Features:**
   - Load sample C# code from the 'TxtCodes' folder.
   - Clear all panels with a single button press.

## Example
1. Paste or load C# code.
2. Obfuscate the code.
3. Compile and run both original and obfuscated code.
4. View results, with successful execution highlighted in green and errors in red.

![image](https://github.com/kamil-caly/Obfuskator_CSharp/assets/66841315/04df9972-eb3a-4309-bd34-734a5a445a64)
*Demonstrating the obfuscation process in the Code Obfuscator application without errors.*

![image](https://github.com/kamil-caly/Obfuskator_CSharp/assets/66841315/93147121-1c45-4ced-9e79-1ef2baeaff0c)
*Demonstrating the obfuscation process in the Code Obfuscator application with some errors.*

## Dependencies
- Visual Studio 2022
- Roslyn Library
