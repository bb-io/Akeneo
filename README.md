
# Blackbird.io Akeneo

Blackbird is the new automation backbone for the language technology industry. Blackbird provides enterprise-scale automation and orchestration with a simple no-code/low-code platform. Blackbird enables ambitious organizations to identify, vet and automate as many processes as possible. Not just localization workflows, but any business and IT process. This repository represents an application that is deployable on Blackbird and usable inside the workflow editor.

## Introduction

<!-- begin docs -->

Akeneo’s PIM (Product Information Management) solution is designed to ingest, normalize, enrich, and centralize product information. It provides enterprise-grade data modeling, governance, and workflows to ensure consistent and high-quality product data management across various channels.

## Before settings up

Before you can connect you need to make sure that:

- You have an API user credentials. You can get them by following this Akeneo guide: https://api.akeneo.com/documentation/authentication.html#api-user-creation

## Connecting

1.  Navigate to Apps, and identify the **Akeneo** app. You can use search to find it.
2.  Click _Add Connection_.
3.  Name your connection for future reference e.g. 'My organization'.
4.  Fill the `Instance URL` field with the url of your Akeneo instance.
5.  Fill in the Username and Password of your API user that you created following the guide above.
6.  When you return to Blackbird, confirm that the connection has appeared and the status is _Connected_.

## Actions

### Products

-   **Delete product** deletes a specific product.
-   **Get product as HTML** returns product values for a specified locale in an HTML format.
-   **Get product info** returns details about a specific product.
-   **Search products** returns a list of products based on filter criteria.
-   **Update product from HTML** updates product content values from a provided HTML file.

## Example

![image](https://github.com/user-attachments/assets/9eb04458-1d84-4b6f-9450-97b59b60c729)

## Missing features

Akeneo is a huge app with a lot of features. If any of these features are particularly interesting to you, let us know!

## Feedback

Do you want to use this app or do you have feedback on our implementation? Reach out to us using the [established channels](https://www.blackbird.io/) or create an issue.

<!-- end docs -->
