FROM microsoft/mssql-server-linux:latest

RUN apt-get -y update && \
    apt-get install -y dos2unix \
    && rm -rf /var/lib/apt/lists/*

RUN mkdir -p /app
WORKDIR /app

COPY . .

# dos2unix is for users who `git clone` on windows
RUN dos2unix *

# Grant permissions for the import-data script to be executable for mac users
RUN chmod +x /app/import-data.sh

CMD /bin/bash ./entrypoint.sh
