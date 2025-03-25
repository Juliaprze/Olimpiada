# Olympic Games Database - Athlete Management System  

A web application for managing Olympic athletes and their participation in events, using SQLite as the database.  

## Features  
- List of athletes with details:  
  - Full name  
  - Weight  
  - Height  
  - Gender  
  - Number of gold, silver, and bronze medals  
  - Number of event participations (linked to detailed view)  

- Detailed view of athlete's participation in events, including:  
  - Sport name  
  - Event name  
  - Olympic Games edition  
  - Season  
  - Athlete's age at the event  
  - Medal won (if applicable)  
  - Link to add a new event participation  

- Form to add a new event participation for an athlete:  
  - Sport name  
  - Event name  
  - Olympic Games edition  
  - Athlete’s age at the event  

## Requirements  
- SQLite database (file must be located at `c:\data\olympics.db`)  
- ASP.NET Core with Entity Framework  
- Authentication (only logged-in users can access forms)  
- Pagination for lists longer than 20 items, including:  
  - First page  
  - Last page  
  - Next page (if applicable)  
  - Previous page (if applicable)  
  - Extended pagination (e.g., `1 2 3 4 ... 10 ... 13 14 15`)  

## Setup  
1. Clone the repository
