FROM microsoft/aspnetcore
RUN apt-get update && apt-get install -y unzip wamerican-insane sqlite3
WORKDIR /app
COPY dist/puzzle-service /app
ENTRYPOINT ["/app/puzzles"]
