
NumberToWords – Technology One Engineering Test

This project is a C# implementation of a number-to-words converter developed as part of the Technology One Engineering Candidate test. The application converts numeric input into its English word representation while adhering to specific grammar, formatting, and rounding rules.

The solution is designed with a clean separation of concerns, robust automated testing, and a simple web-based user interface for interactive testing.

Requirements Summary

The application converts numeric values into words according to the following rules:

Hyphenated values are used for numbers between 21 and 99 (e.g. Twenty-Three)

British-style “And” is used in hundreds (e.g. One Hundred And Twenty-Six)

Correct singular and plural forms are applied (One Dollar vs Two Dollars)

Negative numbers are prefixed with the word Negative

A dot (.) represents the decimal separator

Only digits, an optional leading minus, and a single decimal point are allowed

Values are rounded to two decimal places (away from zero)

Solution Structure

The solution is divided into three main projects:

NumberToWords.Core

A class library containing the core conversion logic implemented as a pure, deterministic function. This layer has no dependency on UI or hosting technologies.

NumberToWords.Core.Tests

An automated test project using xUnit. It contains unit tests and integration tests that verify valid conversions, invalid input handling, boundary conditions, rounding behavior, and end-to-end correctness.

NumberToWords.Web

A Razor Pages web application that provides a simple HTML interface for manually testing the converter.

Technology Stack

Language: C#

Framework: .NET 8

UI: ASP.NET Core Razor Pages

Testing: xUnit

IDE: Visual Studio

How to Run the Web Application

To start the web application locally use the following promt:
dotnet run --project NumberToWords.Web

Design Notes

- The core conversion logic is isolated for reusability and testability

- Grammar rules are implemented explicitly for clarity and correctness

- Automated testing ensures confidence in edge cases and future changes

- Razor Pages were chosen to meet the requirement for an HTML web interface with minimal complexity
