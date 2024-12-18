FROM mcr.microsoft.com/mssql/server:2019-latest

USER root
# 1. Update base packages and install prerequisites
RUN apt-get update \
    && apt-get install -y curl apt-transport-https gnupg2

# 2. Add Microsoft signing key and repository
RUN curl https://packages.microsoft.com/keys/microsoft.asc | apt-key add - \
    && curl https://packages.microsoft.com/config/ubuntu/20.04/prod.list > /etc/apt/sources.list.d/msprod.list

# 3. Update apt and install the tools
RUN apt-get update \
    && ACCEPT_EULA=Y DEBIAN_FRONTEND=noninteractive apt-get install -y mssql-tools unixodbc-dev \
    && echo 'export PATH="$PATH:/opt/mssql-tools/bin"' >> ~/.bashrc

USER mssql