# Blackbird.io Akeneo

Blackbird is the new automation backbone for the language technology industry. Blackbird provides enterprise-scale automation and orchestration with a simple no-code/low-code platform. Blackbird enables ambitious organizations to identify, vet and automate as many processes as possible. Not just localization workflows, but any business and IT process. This repository represents an application that is deployable on Blackbird and usable inside the workflow editor.

## Introduction

<!-- begin docs -->

Akeneo’s PIM (Product Information Management) solution is designed to ingest, normalize, enrich, and centralize product information. It provides enterprise-grade data modeling, governance, and workflows to ensure consistent and high-quality product data management across various channels.

## Before settings up

Before you can connect you need to make sure that:

- You have an active Akeneo instance and you have sufficient rights to add apps to it.
- Go to _Connect_ -> _App Store_ and click on _Create an app_ in the top right corner.
- Give your app a name, for example "Blackbird".
- The activate URL is irrelevant for Blackbird, you can give it the Blackbird login URL like `https://de-1.blackbird.io`.
- For the callback URL fill in `https://bridge.blackbird.io/api/AuthorizationCode`.
- You will receive a `Client ID` and a `Client Secret`. Copy and save these values for the next steps.

![1728308561748](image/README/1728308561748.png)

## Connecting

1.  Navigate to Apps, and identify the **Akeneo** app. You can use search to find it.
2.  Click _Add Connection_.
3.  Name your connection for future reference e.g. 'My Akeneo connection'.
4.  Fill the `Instance URL` field with the url of your Akeneo instance.
5.  Fill in the `Client ID` and `Client Secret` you copied from Akeneo in the previous section.
6.  Click _Authorize connection_ and go through the authentication flow in the popup window.
  - Agree to the privacy policy.
  - Select the _Own_ level, _Edit_ for locales and _Edit_ for attribute groups.
  - Confirm the connection
7.  When you return to Blackbird, confirm that the connection has appeared and the status is _Connected_.

![1728309237542](image/README/1728309237542.png)

## Actions

### General

- **Get all locales** returns a list of locale codes that are available on this Akeneo instance. *Note*: Akeneo does not have a default locale.

### Products

-   **Search products** returns a list of products based on filter criteria, for example product name, categories and updated date.
-   **Delete product** deletes a specific product.
-   **Get product info** returns details about a specific product.
-   **Update product info** updates details of a specific product.

-   **Get product as HTML** returns all localizable product values for a specified locale in an HTML format.
-   **Update product from HTML** updates product content values from a provided HTML file.

### Product models

-   **Search product models** returns a list of product models based on filter criteria, for example code, categories and updated date.
-   **Delete product model** deletes a specific product model.
-   **Get product model info** returns details about a specific product model.
-   **Update product model info** updates details of a specific product model.

-   **Get product model as HTML** returns all localizable product model values for a specified locale in an HTML format.
-   **Update product model from HTML** updates product model content values from a provided HTML file.

All products and product models return information about categories. You can use this to make detailed decisions on localization strategies.

## Events

### Products

-   **On products created or updated** triggers when new products are created or updated. You can filter by locale and categories.
-   **On product models created or updated** triggers when any product models are created or updated. You can filter by locale and categories.

## Example

![image](https://github.com/user-attachments/assets/9eb04458-1d84-4b6f-9450-97b59b60c729)

## Missing features

Akeneo is a huge app with a lot of features. If any of these features are particularly interesting to you, let us know!

In particular we can offer localization capabilities for other aspects of Akeneo like Categories.

## Feedback

Do you want to use this app or do you have feedback on our implementation? Reach out to us using the [established channels](https://www.blackbird.io/) or create an issue.

<!-- end docs -->
