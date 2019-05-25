FROM python:3.6-alpine



RUN mkdir ingester
WORKDIR ingester/
COPY requirements.txt .
COPY Launcher.py .

RUN pip install -r requirements.txt

CMD ["gunicorn", "--bind=0.0.0.0", "Launcher:api"]