# DNP1-Assignment: Main Course Assignment


<b>Feature description</b>:<br />
We need a User, having at least username, and a password. It needs an Id of type int. We need a Post. It is written by a User. It contains a Title and a Body. It also needs an Id, of type int. A User can also write a Comment on a Post. A Comment just contains a Body, and an Id of type int.
All entities must have an Id of type int. The way we create relationships between the Entities is described in detail further below. In short, we use foreign keys, rather than associations.
<br /><br />
<b>As User stories (Prioritized):</b><br />
  1.	As a User, I want to be create posts containing a header (title) and body on the internet forum, so that I can share information with other users.<br />
  2.	As a User, I want to be able to read and comment on posts created by other users, so that information can be discussed between users. Each comment should contain a body of text, that I can write.<br />
  3.	As a User, I want to be able to connect to a internet forum with a personal username and password, so that I might post and comment on the forum.<br />
  4.	As a User, I want to be able to manage (edit/delete) Posts and Comments that I have previously created, so that I can keep information updated and fix potential errors.<br />
  5.	Users want to comment on multiple Posts as well as add multiple comments to Posts, so that as many as possible can collaborate in information sharing.<br />
<br />
<b>As Non-functional requirements:</b><br />
-	Data types:<br />
  o	An id of type Int must be applied to each User class.<br />
  o	Each Post must contain a Title and a Body, as well as an Id of type Int.<br />
  o	Each Comment must contain a Body and an Id of type int.<br />
  o	All entities must have an Id of type int.<br />
