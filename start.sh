# Running the db initialization script
if [[ $INIT_DB == true ]]
    then
        echo "Initializing database..."
        bash init-db.sh

# Runnnig the db reset script
elif [[ $RESET_DB == true ]]
    then
        echo "Resetting database..."
        bash reset-db.sh
fi

# Starting the server
bash run-server.sh;
