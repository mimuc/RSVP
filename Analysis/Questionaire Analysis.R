library(ggplot2)
library(ggstatsplot)
library(tidyverse)
library(ggpubr)
library(rstatix)
library(ARTool)


df <- read.csv("Data/main-tlx-melted.csv", sep = ";")

View(df)

ggwithinstats(df, x = variable, y = value, type = "nonparametric")

df %>% friedman_test(value ~ variable | pid)
ggwithinstats(df, x = variable, y = value, type = "nonparametric")

df <- read.csv("Data/main-questionnaire-melted.csv", sep = ";")

View(df)

df %>% friedman_test(value ~ Participant.ID | pid)
ggwithinstats(df, x = Participant.ID, y = value, type = "nonparametric")
