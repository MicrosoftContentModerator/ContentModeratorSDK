########### Python 2.7 #############
## Sample script for calling the moderator EvaluateImage Api with a url
## To run this script, python 2.7 is required.
####################################
import httplib, urllib, base64

headers = {
    # Request headers
    # Replace the placeholder {Add your Subscription Id} to add your subscription id for this Api, accessible in your profile:
    # https://developer.microsoftmoderator.com/developer
    'Content-Type': 'application/json',
    'Ocp-Apim-Subscription-Key': '{Add your Subscription Id}',
}

params = urllib.urlencode({
})

try:
    ## Replace the placeholder {Add your image Url} below with a url to analize
    conn = httplib.HTTPSConnection('api.microsoftmoderator.com')
    conn.request("POST", "/Image/v2/Evaluate", "{\"Metadata\":[],\"DataRepresentation\":\"Url\",\"Value\":\"{Add your image Url}\"}", headers)
    response = conn.getresponse()
    data = response.read()
    print(data)
    conn.close()
except Exception as e:
    print("[Error {0}] {1}".format(e.args, e.message))

####################################

