# Connect to Amazon RDS instance using an SSL connection

>When you provision a DB instance, Amazon RDS creates an SSL certificate and installs the certificate on the instance. 
These certificates are signed by a Certificate Authority. The SSL certificate includes the DB instance endpoint as 
the Common Name for the SSL certificate to protect the instance against spoofing attacks. An SSL certificate created 
by Amazon RDS is the trusted root entity and works in most cases. However, if your application doesn't accept 
certificate chains, the certificate might fail. In such cases, you might need to use an intermediate certificate to 
connect to your AWS Region. For example, you must use an intermediate certificate to connect to the AWS GovCloud (US) 
Regions using SSL.

>You can download (https://truststore.pki.rds.amazonaws.com/global/global-bundle.pem) a certificate bundle that contains 
both the intermediate and root certificates for all AWS Regions from AWS Trust Services. If your application is on Microsoft 
Windows and requires a PKCS7 file, then you can download the PKCS7 certificate bundle from Amazon Trust Services. This 
bundle contains both the intermediate and root certificates.

>Each database engine has its own process for implementing SSL/TLS. To implement SSL/TLS connection for your DB cluster, 
choose an option based on your database engine.

---
## Important commands:
- Download pem file
  - https://truststore.pki.rds.amazonaws.com/global/global-bundle.pem
- COPY *.pem file to container's cert folder (create one if it doesn't exist, $ `mkdir /usr/local/share/cert`, or use a different folder) 
  - $ `docker cp global-bundle.pem mysql-server:/usr/local/share/cert`
- $ `pwd`
  - /usr/local/share/cert
- $ `mysql -u admin -p -h delete-me-database-100.asdfsdf.us-east-1.rds.amazonaws.com --ssl-ca=global-bundle.pem --ssl-mode=VERIFY_IDENTITY`
---
## Important queries:

```
SELECT 	* 
FROM 	performance_schema.session_status
WHERE	VARIABLE_NAME IN ('Ssl_version', 'Ssl_cipher');
-- WHERE VARIABLE_NAME LIKE '%ssl%';
 
SELECT USER(), CURRENT_USER();
SHOW GRANTS FOR 'admin';
-- ALTER USER 'admin'@'%' REQUIRE SSL;
 
SHOW GLOBAL VARIABLES;
SHOW STATUS;
SHOW VARIABLES LIKE '%ssl%';
SHOW STATUS LIKE 'Ssl_version';
SHOW VARIABLES LIKE '%version%';
SHOW VARIABLES LIKE 'have_ssl';
SHOW SESSION STATUS LIKE '%ssl%';
```
---
## Links:
- [How do I successfully connect to my Amazon RDS instance using an SSL connection?](https://aws.amazon.com/premiumsupport/knowledge-center/rds-connect-ssl-connection/)