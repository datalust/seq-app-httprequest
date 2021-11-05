# Seq.App.Http

Send events and notifications from Seq to a remote HTTP/REST/WebHook endpoint.

**:construction: Under Development :construction:**

## Getting started

Install the app under _Settings > Apps_. The app package id is `Seq.App.Http`.

Visit the Seq documentation for [detailed information about installing and configuring Seq Apps](https://docs.datalust.co/docs/installing-seq-apps).

## Configuration

Instances of the app support the following properties.

| Property | Description | Default |
| --- | --- | --- |
| **URL** | The target URL. May include template substitutions based on event properties, for example, `https://api.example.com/notify?to={Email}`. Placeholders in templates will be URI-encoded. | |
| **Method** | The HTTP method to use. | `POST` |
| **Body** | The request body to send. Template based on event properties, supporting plain text and JSON. | |
| **Media Type** | Media type describing the body | `text/plain` |
| **Authentication Header** | An optional `Name: Value` header, stored as sensitive data, for authentication purposes. | |
| **Other Headers** | Additional headers to send with the request, one per line in `Name: Value` format. | |
| **Extended Error Diagnostics** | Whether or not to include outbound request bodies, URLs, etc., and response bodies when requests fail. | `false` |

## Acknowledgements

Includes substantial portions of [_Serilog.Expressions_](https://github.com/serilog/serilog-expressions), modified to support output encoding.
