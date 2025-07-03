
FROM node:20 AS builder

WORKDIR /app

COPY package*.json ./
RUN npm install


COPY . .
RUN npm run build -- --configuration production

# Nginx
FROM nginx:alpine

# Remove default nginx static assets
RUN rm -rf /usr/share/nginx/html/*


COPY --from=builder /app/dist/404Found/browser /usr/share/nginx/html

EXPOSE 80

CMD ["nginx", "-g", "daemon off;"]
