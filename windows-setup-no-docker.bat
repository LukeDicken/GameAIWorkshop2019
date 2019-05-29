pip install virtualenvwrapper-win
mkvirtualenv ingestion
workon ingestion
pip install falcon gunicorn pandas
mkvirtualenv jupyter
workon jupyter
pip install jupyterlab pandas bokeh