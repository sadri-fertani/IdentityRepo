FROM alexsuch/angular-cli:base

LABEL authors="Sadri FERTANI <sadri.fertani@live.fr>"

RUN apk update \
  && npm install -g @angular/cli@7.3.8 \
  && rm -rf /tmp/* /var/cache/apk/* *.tar.gz ~/.npm \
  && npm cache clear --force \
  && yarn cache clean \
  && sed -i -e "s/bin\/ash/bin\/sh/" /etc/passwd

ADD . /app/

WORKDIR /app

RUN npm install

EXPOSE 4200

VOLUME /app/logs

CMD ng serve --host 0.0.0.0 --port 4200