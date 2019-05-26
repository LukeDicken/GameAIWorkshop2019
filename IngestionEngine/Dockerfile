FROM python:3.6-alpine
RUN mkdir ingester
WORKDIR ingester/
COPY requirements.txt .
RUN pip install -r requirements.txt

COPY Launcher.py .

CMD ["gunicorn", "--bind=0.0.0.0", "Launcher:api"]