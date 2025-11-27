\# Course Registration System (Software Engineering Final Project)



This repository contains the final project for the Software Engineering course:  

a \*\*Course Registration System\*\* implemented by a 3-member development team (BE1, BE2, FE).



---



\## Team Roles 



\- \*\*BE1:\*\* Backend development, initial setup, project structure, DB setup  

\- \*\*BE2:\*\* Backend development, API implementation, authentication, middleware  

\- \*\*FE:\*\* Frontend development, UI/UX, API integration, login \& admin panels  



---



\## Repository 



\- GitHub Repository Name: `software-engineering-final`  

\- All development follows a \*\*feature-based Git workflow\*\*  

\- \*\*Direct push to main is NOT allowed\*\*  

\- All changes must go through \*\*Pull Requests (PRs)\*\* and \*\*Code Review\*\*



---



\## Project Structure 



course-registration/

├── backend/ # Backend source code (Node.js, MVC structure)

├── frontend/ # Frontend source code (React/Vite)

├── docs/ # ERD, API Documentation, Postman collections

├── .github/ # Pull Request Template and GitHub configs

└── README.md





\- `.github/` contains `PULL\_REQUEST\_TEMPLATE.md` (used automatically for all PRs)  

\- `docs/` will contain:  

&nbsp; - ERD of the system  

&nbsp; - API documentation  

&nbsp; - Postman collection  

&nbsp; - Diagrams for use cases / UML  



---



\## Git Workflow 



\### \*\*Main branch rules\*\*

\- No direct commits  

\- Only PR merges  

\- Must stay clean and stable



\### \*\*Feature branches\*\*

\- Each task/feature = one branch  

\- Branch name must follow naming rules below  

\- Work must NEVER be done directly on main



\### \*\*Commits\*\*

\- Small, meaningful, readable messages  

\- Avoid large "bulk" commits



\### \*\*Pull Requests\*\*

\- Must use the PR Template  

\- Must be reviewed by teammates  

\- Merge only after approval  



\### \*\*Branch deletion\*\*

\- After merging → delete the feature branch (recommended)



---



\## Feature Branch Naming Convention


  

\### Backend

feature/BE1-<short-description>

feature/BE2-<short-description>




\### Frontend

feature/FE-<short-description>




\#### Examples:

feature/BE1-db-setup

feature/BE2-auth-api

feature/BE2-course-crud

feature/FE-login-page

feature/FE-admin-dashboard


---


\##  Development Workflow Summary



1\. Pull the latest main:

&nbsp;  ```bash

&nbsp;  git checkout main

&nbsp;  git pull origin main

Create a new feature branch:

git checkout -b feature/<branch-name>

Work on the feature → commit small updates:

git add .

git commit -m "Meaningful update message"

Push your branch:

git push origin feature/<branch-name>

Create Pull Request → use the PR template

Get code review → fix issues if needed

Merge PR into main

Delete the feature branch





