import React, { Component } from 'react';
import { bindActionCreators } from 'redux';
import { connect } from 'react-redux';
import { Link } from 'react-router-dom';
import { actionCreators } from '../store/RabbitMQ';

class FetchRabbitMQMassages extends Component {
    componentDidMount() {
        // This method is called when the component is first added to the document
        this.props.refreshAction();
        this.timeInterval = setInterval(() => {
            this.props.refreshAction();
        }, 1000);
    }

    sendToQ = () => {
        this.props.sendToQAction();
    }

    refresh = () => {
        this.props.refreshAction();
    }

    render() {
        return (
            <div>
                <h1>Messages in Q</h1>
                <p>This component demonstrates RabbitMQ Producer/Consumer</p>
                {renderMessagesTable(this.props, this.refresh, this.sendToQ)}
            </div>
        );
    }
}

function renderMessagesTable(props, refresh, sendToQ) {
    return (
        <div>
            <button className="btn btn-primary" onClick={sendToQ}>SendToQ</button>
            <table className='table table-striped'>
                <thead>
                    <tr>
                        <th>Messages</th>
                    </tr>
                </thead>
                <tbody>
                    {props.messages.map(message =>
                        <tr key={message}>
                            <td>{message}</td>
                        </tr>
                    )}
                </tbody>
            </table>
        </div>
    );
}

export default connect(
    state => state.rabbitMQMessages,
    dispatch => bindActionCreators(actionCreators, dispatch)
)(FetchRabbitMQMassages);
