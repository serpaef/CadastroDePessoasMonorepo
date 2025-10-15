# ===============================
# Stage 1: Build da aplicação
# ===============================
FROM node:20-alpine AS build

WORKDIR /app

COPY package*.json ./
COPY tsconfig*.json ./

RUN npm install

COPY . .

RUN npm run build

# ===============================
# Stage 2: Servir com Nginx
# ===============================
FROM nginx:stable-alpine AS production

# Remover default.conf e substituir pela nossa configuração
RUN rm /etc/nginx/conf.d/default.conf
COPY nginx.conf /etc/nginx/conf.d

# Copiar arquivos buildados para a pasta padrão do Nginx
COPY --from=build /app/dist /usr/share/nginx/html

EXPOSE 80

# Comando padrão do container Nginx
CMD ["nginx", "-g", "daemon off;"]
