1. Attachment tables
2. ComplaintRoles table
3. change the institution to institution_id in "COMPLAINT_MANAGEMENT" table
4. Add kinyarwanda column name to "COMPLAINT_STATUS"
6. `ALTER TABLE ADMIN.PLAINTIFF ADD ASSIGNED_TO CHAR(5);

ALTER TABLE ADMIN.PLAINTIFF
ADD FOREIGN KEY (ASSIGNED_TO) REFERENCES ADMIN.MNGRS_ROLES(ID_ROLE);`
6. 
