# DNP1-Assignment: Main Course Assignment


<b>Feature description</b>:

We need a User, having at least username, and a password. It needs an Id of type int. We need a Post. It is written by a User. It contains a Title and a Body. It also needs an Id, of type int. A User can also write a Comment on a Post. A Comment just contains a Body, and an Id of type int.
All entities must have an Id of type int. The way we create relationships between the Entities is described in detail further below. In short, we use foreign keys, rather than associations.

<b>As User stories (Prioritized):</b>
  1.	As a User, I want to be create posts containing a header (title) and body on the internet forum, so that I can share information with other users.
  2.	As a User, I want to be able to read and comment on posts created by other users, so that information can be discussed between users. Each comment should contain a body of text, that I can write.
  3.	As a User, I want to be able to connect to a internet forum with a personal username and password, so that I might post and comment on the forum.
  4.	As a User, I want to be able to manage (edit/delete) Posts and Comments that I have previously created, so that I can keep information updated and fix potential errors.
  5.	Users want to comment on multiple Posts as well as add multiple comments to Posts, so that as many as possible can collaborate in information sharing.

<b>As Non-functional requirements:</b>
-	Data types:
  o	An id of type Int must be applied to each User class.
  o	Each Post must contain a Title and a Body, as well as an Id of type Int.
  o	Each Comment must contain a Body and an Id of type int.
  o	All entities must have an Id of type int.
