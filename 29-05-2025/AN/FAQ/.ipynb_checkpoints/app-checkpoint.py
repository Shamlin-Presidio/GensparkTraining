from flask import Flask, request, jsonify
import pickle

app = Flask(__name__)

# Load model and vectorizer
with open("faq_model.pkl", "rb") as f:
    vectorizer, clf = pickle.load(f)

# Map predictions to full answers
answers = {
    "create_account": "Use the create account endpoint: /api/Account/create",
    "deposit": "Use the transaction endpoint with type 'Deposit': /api/Account/transaction",
    "check_balance": "This feature is under development.",
    "withdraw": "Withdrawal can be done using the same transaction endpoint with type 'Withdraw'."
}

@app.route('/faq', methods=['POST'])
def answer_question():
    data = request.get_json()
    question = data.get("question", "").lower()

    if not question:
        return jsonify({"error": "No question provided"}), 400

    vec = vectorizer.transform([question])
    pred = clf.predict(vec)[0]
    response = answers.get(pred, "Sorry, I don't understand that question.")

    return jsonify({"answer": response})

if __name__ == '__main__':
    app.run(port=5001)
