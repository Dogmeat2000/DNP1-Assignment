# DNP1-Assignment: Main Course Assignment


<b>Feature description</b>:<br />
We need a User, having at least username, and a password. It needs an Id of type int. We need a Post. It is written by a User. It contains a Title and a Body. It also needs an Id, of type int. A User can also write a Comment on a Post. A Comment just contains a Body, and an Id of type int.
All entities must have an Id of type int. The way we create relationships between the Entities is described in detail further below. In short, we use foreign keys, rather than associations.
<br /><br />
<b>As User stories (Prioritized):</b><br />
  1.	Users want to be create posts containing a header (title) and body on in the forum, so that I can share information with other users.<br />
  2.	Users want to be able to read and comment on posts created by other users, so that information can be discussed between users. Each comment should contain a body of text, that is written by the user.<br />
  3.	Users want to be able to connect to a the forum with a personal username and password, so that different users can be distinguished and different users can be assignes as authors on the created posts and comments<br />
  4.	Users want to be able to manage (edit/delete) Posts and Comments that the user has previously created, so that the user can keep information updated and fix potential errors or misspelling.<br />
  5.	Users want to comment on multiple Posts, as well as add multiple comments to each Post, so that as many as possible can collaborate in the information sharing.<br />
  6.  Users want to be able to create main forums, as well as sub-forums, so that posts with related themes/topics can be collected inside containing forums and subforums for ease of finding posts with related topics.<br />
<br />
<b>As Non-functional requirements:</b><br />
-	Data types:<br />
  o	An id of type Int must be applied to each User class.<br />
  o	Each Post must contain a Title and a Body, as well as an Id of type Int.<br />
  o	Each Comment must contain a Body and an Id of type int.<br />
  o	All entities must have an Id of type int.<br />
