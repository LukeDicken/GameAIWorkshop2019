FROM jupyter/scipy-notebook
RUN rm -rf work
VOLUME shared/
WORKDIR shared/
CMD ["jupyter","lab"]