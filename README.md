# StudentsManager

General Idea
	This is an exercise to see how you write and structure code. There is no right or wrong answer, only design choices. We realize that this is somewhat of a trivial example, so please do not feel the need to over engineer your work. For instance, there is no need at to implement a frontend of any kind. However, please do adhere to what you consider to be good coding practices. Show mastery of object oriented patterns.

Project Overview & Requirements
	You are modeling a system to manage students that will be used by high schools, elementary schools, kindergardens, etc.
	Your task is to create the business objects to manage the students in the system:
		 Store the students in the system
		 Create new students
		 Delete a specific student
		 Search for students in ways that make sense for the clients
			o By name, sorted alphabetically
			o By student type (kinder, elementary, high, university) sorting by date, most recent to least recent.
			o By gender and type (female elementary) sorting by date, most recent to least recent.
Your solution should focus on the server objects necessary to implement the core functionality of managing the students and searching for students.
You are not expected to use a DB, or create a GUI.

If this were a real application, the search request would originate from a web page. Your student management classes would be invoked to perform the search (perhaps even as a standalone web service) and the results returned to a web page. To simplify this example, the request will come from the command line, and the results will be returned to the console.
Keep in mind that your solution needs to be extensible & performant as if we were going to drop it into a web environment for a large educative organization; Although your sample may only utilize 10 students, it should be able to efficiently handle 50,000 or more. And hundreds of simultaneous requests that you would expect to find in a web based application; i.e., students being simultaneously searched for, created, and updated.
To simulate a web based operating environment for this exercise, the command line should be used to:
	 Read students at system startup from a CSV input file as well as reading the search request from the command line
		Ex: studentSolution.exe input.csv name=leia
		Ex: studentSolution.exe input.csv type=kinder
		Ex: studentSolution.exe input.csv type=elementary gender=female
	 Use the create student methods to create each student from the CSV file in your business objects.
	 Echo the results of the requested search operation to the console.

The CSV File
	Given an unordered CSV file with EOL terminated lines of the format:
	Kinder,Leia,F,20151231145934
	High,Luke,M,20130129080903
	Where each line has the format:
	Student Type, Student Name, Gender, Timestamp of last update in the system
	The “Timestamp” format is: “<year><month><day><hour><minute><second>”, e.g.:
	The representation for December 31st, 2013 14:59:34 is 20131231145934.
	Add additional data lines to the file to illustrate your solution works.
	Please include any questions and/or thoughts you had that may have influenced your design along with
	your solution.