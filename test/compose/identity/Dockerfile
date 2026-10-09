# Keycloak with the Evently realm imported on start. realm-export.json is copied here at build time from .files/realm-export.json
FROM quay.io/keycloak/keycloak:26.7
COPY realm-export.json /opt/keycloak/data/import/realm.json
