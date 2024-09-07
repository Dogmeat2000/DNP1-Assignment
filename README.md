# DNP1-Assignment: Main Course Assignment


<h2><b>Mandatory feature description</b>:</h2>
<p>We need a User, having at least username, and a password. It needs an Id of type int. We need a Post. It is written by a User. It contains a Title and a Body. It also needs an Id, of type int. A User can also write a Comment on a Post. A Comment just contains a Body, and an Id of type int.
All entities must have an Id of type int. The way we create relationships between the Entities is described in detail further below. In short, we use foreign keys, rather than associations.</p>

<h2><b>As User stories (Prioritized):</b></h2>
  1.	<b>Mandatory:</b> Users want to be create posts containing a header (title) and body on in the forum, so that I can share information with other users.<br />
  2.	<b>Mandatory:</b> Users want to be able to read and comment on posts created by other users, so that information can be discussed between users. Each comment should contain a body of text, that is written by the user.<br />
  3.	<b>Mandatory:</b> Users want to be able to connect to a the forum with a personal username and password, so that different users can be distinguished and different users can be assigned as authors on the created posts and comments<br />
  4.  <b>Optional:</b> Users want to be able to choose their own username and password, as well as modify these later, so that users may keep their personal information up to date and improve security by allowing passwords to be updated<br />
  5.	<b>Optional:</b> Users want to be able to manage (edit/delete) Posts and Comments that the user has previously created, so that the user can keep information updated and fix potential errors or misspelling.<br />
  6.	<b>Optional:</b> Users want to comment on multiple Posts, as well as add multiple comments to each Post, so that multiple users can collaborate in the information sharing.<br />
  7.  <b>Optional:</b> Users want to be able to create main forums, as well as sub-forums, so that posts with related themes/topics can be collected inside containing forums and subforums for ease of finding posts with related topics.<br />
  8.  <b>Optional:</b> Users want to be warned when attempting to edit/delete previously created posts and comments, so that users do not edit/delete anything without informed consent.<br />
  9.  <b>Optional:</b> Users want to be able to see and restore deleted posts within 30 days of deletion, so that mistakes can be undone.<br />
  10.  <b>Optional:</b> Users want to be able to see the date and time of each post and comment, so that users are able to identify new and old informtation.<br />
  11.  <b>Optional:</b> Users want to be able to see the date and time of the most recent post or comment in each forum and sub-forum, so that users have a fast means of identifying if changes were made inside the forums they are interested in.<br />
<br />


<h2><b>As Non-functional requirements:</b></h2>
-	Data types:<br />
  o	An id of type Int must be applied to each User class.<br />
  o	Each Post must contain a Title and a Body, as well as an Id of type Int.<br />
  o	Each Comment must contain a Body and an Id of type int.<br />
  o	All entities must have an Id of type int.<br /><br />
- Rules when viewing the forum:<br />
  1.	Users should be able to see if any post or comment was edited.<br />

  
<h2><b>Domain Model:</b></h2>
